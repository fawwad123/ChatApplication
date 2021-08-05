import React from 'react'
import Moment from 'react-moment';
import './ConversationItem.scss'

const ConversationItem = (props) => {
    const {id, imageUrl, imageAlt, title, date, message} = props.conversation
    let className = 'conversation'
    
    if(props.isActive)
        className += ' active'
    
    const length = date.length-1;
    const handleChange = (id) => {
        props.selectConversationIndex(id);
    }
    
    return (
        <div className={className} id={id} onClick={(e) => {handleChange(e.target.id)}}>
            <img src={imageUrl} alt={imageAlt} id={id} />
            <div className="title-text" id={id}>{title}</div>
            <div className="created-date" id={id}>
                {length === -1 
                ? ''
                : <Moment format='YYYY/MM/DD' id={id}>{date[length]}</Moment>}
            </div>
            <div className="conversation-message" id={id}>{message[length]}</div>
        </div>
    )
}

export default ConversationItem;