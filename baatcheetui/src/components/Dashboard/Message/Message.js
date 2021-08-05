import React from 'react'
import './Message.scss'
import Moment from 'react-moment';

const Message = (props) => {
    let messageClassName = 'message-row'
    let imageThumbnail = null

    if(props.isMyMessage)
        messageClassName += ' you-message'
    else{
        messageClassName += ' other-message'
        imageThumbnail = <img src={props.messages.imageUrl} alt={props.messages.imageAlt} />
    }
    return (
        <div className={messageClassName}>
            <div className="message-content">
                {imageThumbnail}
                <div className="message-text">{props.messages.message}</div>
                <div className="message-time"><Moment format="YYYY/DD/MM hh:mm:ss">{props.messages.messageOn}</Moment></div>
            </div>
        </div>
    )
}

export default Message;