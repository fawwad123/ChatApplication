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
    const [userProfile, setUserProfile] = useState({});
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
    const [modalMessage, setModalMessage] = useState("")
    const [searchValue, setSearchValue] = useState("")
    const latestChat = useRef(null);
    latestChat.current = messages;
    
    useEffect(() => {
        document.body.style.display = 'grid'
        if(sessionStorage.getItem('token') && user.person === undefined)
            dispatch(getUserProfile());
        else if(sessionStorage.getItem('token') === null && isAuth === false)
            history.push('/');
        
    }, [messages, dispatch, history, isAuth, user.person])

    const connectFriend = async (newUserContactId) =>{
        try {
            if(connection !== null && connection.connectionState === 'Connected'){
                await connection.invoke("JoinFriend",userContactId, newUserContactId).catch(err => console.error(err));
            }
        }
        catch(e) {
            console.log('Sending message failed.', e);
        }
    }

    const connectGroup = async (newGroupId) =>{
        try {
            if(connection !== null && connection.connectionState === 'Connected')
                await connection.invoke("JoinGroup",groupId, newGroupId).catch(err => console.error(err));
        }
        catch(e) {
            console.log('Sending message failed.', e);
        }
    }
    
    const sendMessage = async (message) => {
        try {
            if(connection.connectionState === 'Connected')
                if(userContactId === -1)
                    await connection.invoke("SendMessageToGroup", message, groupId).catch(err => console.error(err));
                else
                    await connection.invoke("SendMessageToFriend", message, userContactId, user.person.id).catch(err => console.error(err));
            setConnection(connection);
        }
        catch(e) {
            console.log('Sending message failed.', e);
        }
    }

    const signOut = async () => {
        sessionStorage.removeItem('token');
        history.push('/');
        dispatch(clearUser())
        await connection.stop()
    }

    var selectConversationIndex = (id) => {
        setSelectedId(id)
        setPerson(user.person)
        setIsContact(true)
        setAddContact(false)
        messages[0].map((chatDetails) => {
            if(id == chatDetails.person.id){
                setUserContactId(chatDetails.userContactId)
                setGroupId(-1)
                setSelectedConversation(chatDetails.person.name)
                connectFriend(chatDetails.userContactId)
            }
        });
    }

    var selectGroupConversationIndex = (id) => {
        setSelectedId(id)
        setPerson(userProfile.person)
        setIsContact(false)
        setAddContact(false)
        messages[1].map((groupDetails) => {
            if(id == groupDetails.groupId){
                setUserContactId(-1);
                setGroupId(groupDetails.groupId)
                setSelectedConversation(groupDetails.groupName);
                connectGroup(groupDetails.groupId)
            }
        });
        console.log("Messages", messages)
    }

    const connectServer = async () => {
        try {
            if(connection == null && user.person !== undefined){
                const connection = new HubConnectionBuilder()
                .withUrl('https://localhost:5001/hubs/chathub')
                .withAutomaticReconnect()
                .build();
                
                await connection.start()
                    .then(result => {
                        console.log('Connected!');
                        connection.on('ReceiveMessage', message => {
                            try {
                                const updatedChat = [...latestChat.current];
                                const select = message.groupId === undefined ? true : false;
                                var m = updatedChat[select ? 0 : 1].map(conversation => {
                                    var msgs = Object.assign([], select ? conversation.messages : conversation.chats);
                                    if(conversation && conversation.userContactId == message.userContactId && conversation.userContactId !== undefined){
                                        msgs.push(message)
                                        conversation = {...conversation, messages: msgs}
                                        return conversation;
                                    }
                                    if(conversation && conversation.groupId == message.groupId && conversation.groupId !== undefined){
                                        msgs.push(message)
                                        conversation = {...conversation, chats: msgs}
                                        return conversation;
                                    }
                                });
                                
                                var chat = updatedChat[select ? 0 : 1].map( obj => {
                                    for(var i in m)
                                        if(m[i] !== undefined){
                                            if(select && obj.userContactId === m[i].userContactId) {
                                                return m[i];
                                            }
                                            if(!select && obj.groupId === m[i].groupId) {
                                                return m[i];
                                            }
                                        }
                                    return obj
                                })
                                updatedChat[select ? 0 : 1] = chat
                                setMessages(updatedChat);
                            } catch (error) {
                                console.log(error);
                            }
                        });
                        connection.on('NewGroup', (group, message) => {
                            try {
                                const updatedChat = [...latestChat.current];
                                updatedChat[1].push(group)
                                setMessages(updatedChat);
                                setModalMessage(message)
                            } catch (error) {
                                console.error(error);
                            }
                        })
                        connection.on('NewContact', (contact, message) =>{
                            try {
                                const updatedChat = [...latestChat.current];
                                updatedChat[0].push(contact)
                                setMessages(updatedChat);
                                setModalMessage(message)
                            } catch (error) {
                                console.error(error);
                            }
                        })
                        connection.on('NewContactGroup', (group, message) => {
                            try {
                                const updatedChat = [...latestChat.current];
                                var isGroup = false;
                                var groupChat = updatedChat[1].map(userGroup => {
                                    if(userGroup.groupId == group.groupId){
                                        isGroup = true;
                                        var grpMem = Object.assign([], group.groupMember)
                                        userGroup = {...userGroup, groupMember: grpMem};
                                    }
                                    return userGroup
                                })
                                if(isGroup)
                                    updatedChat[1] = groupChat
                                else
                                    updatedChat[1].push(group)
                                    
                                setMessages(updatedChat)
                                setModalMessage(message)
                                console.log(group);
                            } catch (error) {
                                console.error(error);
                            }
                        })
                    })
                    .catch(e => console.log('Connection failed: ', e));
                    await connection.invoke("LoginUser",user.person.id).catch(err => console.error(err));
                    setConnection(connection);
            }
        } catch (error) {
            console.log(error)
        }
    }

    const addNewContact = async (contact) => {
        await connection.invoke("AddNewContact", contact.email, contact.userId).catch(err => console.error(err));
    }

    const addContactToGroup = async (contactGroup) => {
        await connection.invoke("AddContactToGroup", contactGroup.email, contactGroup.addedBy, contactGroup.groupId).catch(err => console.error(err));
    }

    const addNewGroup = async (group) => {
        await connection.invoke("AddNewGroup", group.name, group.createdBy).catch(err => console.error(err));
    }

    connectServer()
    var contacts, groups, conversations, groupConversation;
    if(user.person !== undefined && userProfile.person === undefined){
        setUserProfile(user)
        contacts = user.chatDetails.map((chatDetails) => {
            return chatDetails
        })
        groups = user.groupDetails.map((groupDetails) => {
            return groupDetails
        })
        
        setMessages([contacts, groups])
    
    }
    if(messages.length > 0){
        conversations = messages[0].map((chatDetails) => {
            return(
                {
                    id: chatDetails.person.id,
                    imageUrl: chatDetails.person.imageUrl,
                    imageAlt: chatDetails.person.name,
                    title: chatDetails.person.name,
                    date: chatDetails.messages.map(a => a.messageOn),
                    message: chatDetails.messages.map(a => a.message)
                }
            )
            
        });

        groupConversation = messages[1].map((groupDetails) => {
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
            <NewConversation 
                setModalIsOpen={setModalIsOpen} 
                modalIsOpen={modalIsOpen} 
                setIsContact={setIsContact}
                setAddContact={setAddContact}/>
            <ChatTitle 
                selectedConversation = {selectedConversation}
                signOut = {signOut}
                isContact = {isContact}
                setModalIsOpen={setModalIsOpen} 
                modalIsOpen={modalIsOpen}
                setAddContact={setAddContact}
                person={user.person}
            />
            {messages.length > 0  && selectedId !== 0 ? <MessageList 
                userId={person ? person.id : null} 
                messages = {messages}
                isContact = {isContact}
                selectedId = {selectedId}
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
            addContact={addContact}
            addNewContact={addNewContact}
            addContactToGroup={addContactToGroup}
            addNewGroup={addNewGroup}
            setModalMessage={setModalMessage}
            modalMessage={modalMessage}
            selectedId = {selectedId} />
        </>
    )
}

export default Dashboard;