import React from 'react';
import './ChatTitle.scss';
import 'font-awesome/css/font-awesome.min.css';
import Notifications from "react-notifications-menu"
import img from '../../../images/profiles/ben.png'
const ChatTitle = (props) => {
    const {selectedId, signOut, isContact, selectedConversation, setModalIsOpen, modalIsOpen, setAddContact} = props
    const data = [
        {
            image: img,
            message: 'Kameshwaran S had shared a feedback with you.',
            detailPage: '/',
        },
    ]
    return (
        (selectedId !== 0)
        ?<div id="chat-title">
            <span>{selectedConversation}</span>
            <div id="icon">
                {selectedConversation === "" || isContact ? '':
                <i className="fa fa-plus" aria-hidden="true" onClick={() => {setModalIsOpen(!modalIsOpen); setAddContact(true)}}></i>}
                <Notifications data={data} height={250} />
                <i className="fa fa-sign-out" aria-hidden="true" onClick={() => signOut()}></i>
            </div>
        </div>
        :''
    );
}

export default ChatTitle;