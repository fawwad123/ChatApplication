import React from 'react';

import './MessageList.scss';
import Message from './Message';

const MessageList = (props) => {
    const {messages, userId, selectedId, isContact} = props

    const messageItems = !messages ? '' : messages[isContact ? 0 : 1].map((message, index) => {
        if(message !== undefined && message !== null) {
            if(isContact){
                if(message.person.id == selectedId) {
                    return message.messages.slice(0).reverse().map((message, index) => {
                        return( 
                            <Message 
                                key={index}
                                isMyMessage = {userId === message.messageBy}
                                imageAlt = {message.imageAlt}
                                imageUrl = {message.imageUrl}
                                messages = {message}
                            />
                        )
                    })
                }
            }
            else{
                if(message.groupId == selectedId) {
                    return message.chats.slice(0).reverse().map((chat, index) => {
                        return( 
                            <Message 
                                key={index}
                                isMyMessage = {userId === chat.messageBy}
                                imageAlt = {chat.imageAlt}
                                imageUrl = {chat.imageUrl}
                                messages = {chat}
                            />
                        )
                    })
                }
            }
        }
    })
    return (
        (selectedId !== 0)
        ?
        <>
            
                <div id="chat-message-list">
                    {messageItems}
                </div>
        </>:''
    );
}

export default MessageList;