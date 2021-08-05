import React from 'react';

import './MessageList.scss';
import Message from './Message';

const MessageList = (props) => {
    const {messages, userId, selectedId, isLoadingMessage} = props

    const messageItems = !messages ? '' : messages.map((message, index) => {
        if(message !== undefined && message !== null)
            return message.message.slice(0).reverse().map((message, index) => {
                return( 
                    <Message 
                        key={index}
                        isMyMessage = {userId === message.messageBy}
                        imageAlt = {message.imageAlt}
                        imageUrl = {message.imageUrl}
                        messages = {message}
                        isLoadingMessage = {isLoadingMessage}
                    />
                )
            })
        return null
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