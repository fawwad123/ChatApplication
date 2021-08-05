import React from 'react';
import './NoConversations.scss';

const NoConversations = (props) => {
    const {modalIsOpen, setModalIsOpen, setIsContact} = props;
    return (
        <div id="no-conversation-layout">
            <div id="no-conversation-content">
                <h2>No Conversations</h2>
                <p>Currently you have not selected any conversations.</p>
                <p>To start a new conversation click the button below.</p>
                <button className="primary-button" 
                    onClick={() => {
                        setModalIsOpen(!modalIsOpen)
                        setIsContact(true)
                    }}>New Conversation</button>
            </div>
        </div>
    );
}

export default NoConversations;