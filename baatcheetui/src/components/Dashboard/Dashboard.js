import React, { useState, useEffect, useRef } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { useDispatch, useSelector } from 'react-redux';
import { useHistory } from 'react-router-dom';
import { getUserProfile } from '../../actions/userAction';
import { clearUser } from '../../slice/userSlice';
import ConversationList from './Conversation/ConversationList';
import NewConversation from './Conversation/NewConversation';
import ChatTitle from './Chat-Title/ChatTitle';
import MessageList from './Message/MessageList';
import ChatForm from './Chat-Form/ChatForm';
import './Dashboard.scss';
import ConversationSearch from './Conversation/ConversationSearch';
import daryl from '../../images/profiles/daryl.png';
import NoConversations from './Conversation/NoConversation';
import CustomModal from './Modal/Modal';

const Dashboard = () => {
    const history = useHistory();
    const dispatch = useDispatch();
    const {isAuth} = useSelector((state) => state.login);
    const {user} = useSelector((state) => state.user)
    const [connection, setConnection] = useState(null);
    const [selectedId, setSelectedId] = useState(0);
    const [messages, setMessages] = useState([]);
    const [userContactId, setUserContactId] = useState(0)
    const [groupId, setGroupId] = useState(0)
    const [person, setPerson] = useState(null)
    const [modalIsOpen, setModalIsOpen] = useState(false)
    const [selectedConversation, setSelectedConversation] = useState("")
    const [isContact, setIsContact] = useState(true)
    const [addContact, setAddContact] = useState(false)
    const [searchValue, setSearchValue] = useState("")
    const latestChat = useRef(null);
    latestChat.current = messages;
    
    useEffect(() => {
        document.body.style.display = 'grid'
        if(sessionStorage.getItem('token'))
            dispatch(getUserProfile());
        else if(sessionStorage.getItem('token') === null && isAuth === false)
            history.push('/');
        try {
            
            const connection = new HubConnectionBuilder()
            .withUrl('https://localhost:5001/hubs/chathub')
            .withAutomaticReconnect()
            .build();
            
            connection.start()
                .then(result => {
                    console.log('Connected!');

                    connection.on('ReceiveMessage', message => {
                        const updatedChat = [...latestChat.current];
                        var userContact = sessionStorage.getItem('userContact');
                        var group = sessionStorage.getItem('groupId');
                        var m = updatedChat.map(messages => {
                            if(messages){
                                var msgs = Object.assign([], messages.message);
                                if(message.userContactId == userContact && message.userContactId !== undefined)
                                    msgs.push(message)
                                if(message.groupId == group && message.groupId !== undefined)
                                    msgs.push(message)
                                messages.message = msgs 
                                return messages;
                            }
                            return null
                        });
                        setMessages(m);
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
            setConnection(connection);
        } catch (error) {
            console.log(error)
        }
    }, [messages, dispatch, history, isAuth])

    const connectFriend = async (newUserContactId) =>{
        try {
            if(connection.connectionState === 'Connected')
                connection.invoke("JoinFriend",userContactId, newUserContactId).catch(err => console.error(err));
        }
        catch(e) {
            console.log('Sending message failed.', e);
        }
    }
    const connectGroup = async (newGroupId) =>{
        try {
            if(connection.connectionState === 'Connected')
                connection.invoke("JoinGroup",groupId, newGroupId).catch(err => console.error(err));
        }
        catch(e) {
            console.log('Sending message failed.', e);
        }
    }
    const sendMessage = async (message) => {
        try {
            if(connection.connectionState === 'Connected')
                if(userContactId === -1)
                    connection.invoke("SendMessageToGroup", message, groupId).catch(err => console.error(err));
                else
                    connection.invoke("SendMessageToFriend", message, userContactId).catch(err => console.error(err));
            setConnection(connection);
        }
        catch(e) {
            console.log('Sending message failed.', e);
        }
    }

    const signOut = () => {
        sessionStorage.removeItem('token');
        history.push('/');
        dispatch(clearUser())
    }

    var conversations, groupConversation;
    if(user.chatDetails){
        conversations = user.chatDetails.map((chatDetails) => {
            return(
                {
                    id: chatDetails.person.id,
                    imageUrl: daryl,
                    imageAlt: chatDetails.person.name,
                    title: chatDetails.person.name,
                    date: chatDetails.messages.map(a => a.messageOn),
                    message: chatDetails.messages.map(a => a.message)
                }
            )
        });
    }
    if(user.groupDetails){
        groupConversation = user.groupDetails.map((groupDetails) => {
            return({
                id: groupDetails.groupId,
                imageUrl: daryl,
                imageAlt: groupDetails.groupName,
                title:  groupDetails.groupName,
                date: groupDetails.chats.map(a => a.messageOn),
                message: groupDetails.chats.map(a => a.message)
            })
        })
    }
    
    var selectConversationIndex = (id) => {
        setSelectedId(id)
        setPerson(user.person)
        setIsContact(true)
        setAddContact(false)
        var messages = user.chatDetails.map((chatDetails) => {
            if(id == chatDetails.person.id){
                setUserContactId(chatDetails.userContactId);
                setGroupId(-1)
                sessionStorage.setItem('userContact', chatDetails.userContactId);
                sessionStorage.removeItem('groupId');
                setSelectedConversation(chatDetails.person.name);
                connectFriend(chatDetails.userContactId)
                return(
                    {
                        id: chatDetails.person.id,
                        imageUrl: daryl,
                        imageAlt: chatDetails.person.name,
                        message: chatDetails.messages,
                    }
                )
            }
            return null;
        });
        console.log("Messages", messages)
        setMessages(messages);
    }

    var selectGroupConversationIndex = (id) => {
        setSelectedId(id)
        setPerson(user.person)
        setIsContact(false)
        setAddContact(false)
        var messages = user.groupDetails.map((groupDetails) => {
            if(id == groupDetails.groupId){
                setUserContactId(-1);
                setGroupId(groupDetails.groupId)
                sessionStorage.setItem('groupId', groupDetails.groupId);
                sessionStorage.removeItem('userContact');
                setSelectedConversation(groupDetails.groupName);
                connectGroup(groupDetails.groupId)
                return(
                    {
                        id: groupDetails.groupId,
                        imageUrl: daryl,
                        imageAlt: groupDetails.groupName,
                        message: groupDetails.chats,
                    }
                )
            }
            return null;
        });
        console.log("Messages", messages)
        setMessages(messages);
    }

    
    return (
        <>
        <div id="chat-container">
            <ConversationSearch setSearchValue={setSearchValue}/>
            <ConversationList 
                conversations = {conversations} 
                groupConversations = {groupConversation}
                selectConversationIndex = {selectConversationIndex}
                selectGroupConversationIndex = {selectGroupConversationIndex}
                selectedId = {selectedId}
                searchValue = {searchValue}
            />
            <NewConversation setModalIsOpen={setModalIsOpen} modalIsOpen={modalIsOpen} setIsContact={setIsContact}/>
            <ChatTitle 
                selectedConversation = {selectedConversation}
                signOut = {signOut}
                isContact = {isContact}
                setModalIsOpen={setModalIsOpen} 
                modalIsOpen={modalIsOpen}
                setAddContact={setAddContact}
            />
            {messages.length > 0  && selectedId !== 0 ? <MessageList 
                userId={person ? person.id : null} 
                messages = {messages}
            />:
            <NoConversations 
                setModalIsOpen={setModalIsOpen} 
                modalIsOpen={modalIsOpen} 
                setIsContact={setIsContact}/>}
            <ChatForm 
                selectedId = {selectedId} 
                userContactId = {userContactId}
                groupId = {groupId}
                sendMessage = {sendMessage}
                userId = {person ? person.id : null}/>
        </div>
        <CustomModal 
            modalIsOpen={modalIsOpen} 
            setModalIsOpen={setModalIsOpen} 
            isContact={isContact}
            addContact={addContact}/>
        </>
    )
}

export default Dashboard;