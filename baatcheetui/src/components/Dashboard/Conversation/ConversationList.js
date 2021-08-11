import React, { useState } from 'react'
import './ConversationList.scss'
import ConversationItem from './ConversationItem';
import 'font-awesome/css/font-awesome.min.css';
import GroupConversationItem from './GroupConversationItem';

const ConversationList = (props) => {
    const {selectConversationIndex, selectedId, conversations, groupConversations, selectGroupConversationIndex, searchValue}  = props;
    const [showContacts, setShowContacts] = useState(true)
    const [showGroups, setShowGroups] = useState(true)
    const conversationItems = !conversations ? '' : conversations.map((conversation, index) => {
        if(conversation.title.includes(searchValue) || searchValue === "")
            return (
                <ConversationItem 
                    key = {conversation.id} 
                    isActive = {conversation.id === selectedId}
                    conversation = {conversation} 
                    selectConversationIndex = {selectConversationIndex}/>
            )
        return null
    })
    const groupConversationItems = !groupConversations ? '': groupConversations.map((groupConversation, index) => {
        if(groupConversation.title.includes(searchValue) || searchValue === "")
            return(
                <GroupConversationItem
                    key = {groupConversation.id}
                    isActive = {false}
                    groupConversation = {groupConversation} 
                    selectGroupConversationIndex = {selectGroupConversationIndex}
                />
            )
        return null
    })
    return (
        <div id="conversation-list">
            <div 
                className="title-text" 
                style={{flex:1,alignContent:'center', backgroundColor: '#0462a1', fontSize:15}}
                onClick={() => setShowGroups(!showGroups)}>
                {showGroups ? <i className="fa fa-caret-down" aria-hidden="true"></i> : <i className="fa fa-caret-right" aria-hidden="true"></i>} Groups
            </div>
            {showGroups ? groupConversationItems : ''}
            <div 
                className="title-text" 
                style={{flex:1,alignContent:'center', backgroundColor: '#0462a1', fontSize:15}}
                onClick={() => setShowContacts(!showContacts)}>
                {showContacts ? <i className="fa fa-caret-down" aria-hidden="true"></i> : <i className="fa fa-caret-right" aria-hidden="true"></i>} Contacts
            </div>
            {showContacts ? conversationItems : ''}
        </div>
    );
}

export default ConversationList;