import React from 'react';
import './ChatTitle.scss';
import 'font-awesome/css/font-awesome.min.css';
import { DropdownMenu } from './DropdownMenu';
import { NavIcon } from './NavIcon';

const ChatTitle = (props) => {
    const {selectedId, signOut, isContact, selectedConversation, setModalIsOpen, modalIsOpen, setAddContact, person} = props
    return (
        (selectedId !== 0)
        ?<div id="chat-title">
            <span>{selectedConversation}</span>
            <div id="icon">
                {selectedConversation === "" || isContact ? '':
                <i className="fa fa-plus" aria-hidden="true" onClick={() => {setModalIsOpen(!modalIsOpen); setAddContact(true)}}></i>}
                <NavIcon>
                    <DropdownMenu
                        signOut={signOut}
                        person={person}/>
                </NavIcon>
            </div>
        </div>
        :''
    );
}

export default ChatTitle;