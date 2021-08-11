import React from 'react'
import './NewConversation.scss';
import 'font-awesome/css/font-awesome.min.css';

const NewConversation = (props) => {
    const {modalIsOpen, setModalIsOpen, setIsContact, setAddContact} = props;
    return (
        <div id="new-message-container">
            <span 
                className="fa fa-user-plus" 
                aria-hidden="true" 
                onClick={() => {
                    setModalIsOpen(!modalIsOpen)
                    setIsContact(true)
                    setAddContact(true)
                }}/>
            <span 
                className="fa fa-users" 
                aria-hidden="true"
                onClick={() => {
                    setModalIsOpen(!modalIsOpen)
                    setIsContact(false)
                    setAddContact(false)
                }}/>
        </div>
    )
}

export default NewConversation;