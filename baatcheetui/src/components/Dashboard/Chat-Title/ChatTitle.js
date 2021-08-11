import React from 'react';
import './ChatTitle.scss';
import 'font-awesome/css/font-awesome.min.css';

const ChatTitle = (props) => {
    const {selectedId, signOut, isContact, selectedConversation, setModalIsOpen, modalIsOpen, setAddContact} = props
    return (
        (selectedId !== 0)
        ?<div id="chat-title">
            <span>{selectedConversation}</span>
            <div id="icon">
                {selectedConversation === "" || isContact ? '':
                <i className="fa fa-plus" aria-hidden="true" onClick={() => {setModalIsOpen(!modalIsOpen); setAddContact(true)}}></i>}
                <i className="fa fa-sign-out" aria-hidden="true" onClick={() => signOut()}></i>
            </div>
        </div>
        :''
    );
}

export default ChatTitle;