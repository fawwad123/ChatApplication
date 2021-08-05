import React, {useState} from 'react';
import './ChatForm.scss';
import 'font-awesome/css/font-awesome.min.css';

const ChatForm = (props) => {
    const {selectedId, sendMessage, userId, userContactId, groupId} = props
    const [textMessage, setTextMessage] = useState('');

    const conversationHandleKeyDown = (event) => {
        if (event.key === 'Enter' && event.target.value !== "") {
            var message = {
                isDeleted: false,
                message: event.target.value,
                messageBy: userId, 
                messageOn: new Date(),
                userContactId: userContactId
            }
            sendMessage(message)
            setTextMessage("")
        }
    }

    const groupConversationHandleKeyDown = (event) => {
        if (event.key === 'Enter' && event.target.value !== "") {
            var message = {
                isDeleted: false,
                message: event.target.value,
                messageBy: userId, 
                messageOn: new Date(),
                groupId: groupId
            }
            sendMessage(message)
            setTextMessage("")
        }
    }
    
    const handleChange = (e) => {
        e.persist();
        setTextMessage(e.target.value);
    }

    const handleSubmit = (e) => {
        if(e)  e.preventDefault();
        var message = {};
        if(textMessage !== ""){
            if(userContactId === -1)
            message = {
                isDeleted: false,
                message: textMessage,
                messageBy: userId, 
                messageOn: new Date(),
                groupId: groupId
            }
            else
                message = {
                    isDeleted: false,
                    message: textMessage,
                    messageBy: userId, 
                    messageOn: new Date(),
                    userContactId: userContactId
                }

            sendMessage(message)
            setTextMessage("")
        }
    };

    return (
        selectedId !== 0
        ?
        <>
        <div className="form" id="chat-form">
            <input 
                name="message"
                type="text" 
                placeholder="type a message" 
                onKeyDown={(e) => userContactId === -1 ? groupConversationHandleKeyDown(e) : conversationHandleKeyDown(e)}
                value = {textMessage || ''}
                onChange={(e) => handleChange(e)}
            />
            <button type="submit" onClick={(e) => handleSubmit(e)}>Send</button>
        </div>
        </>
        :''
    );
}

export default ChatForm;