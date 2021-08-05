import React, { useState } from 'react'
import './ConversationSearch.scss'

const ConversationSearch = (props) => {
    const [value, setValue] = useState("");
    const handleChange = (e) => {
        setValue(e.target.value);
        props.setSearchValue(e.target.value)
    }
    return (
        <div id="search-container">
            <input type="text" placeholder="Search" value={value} onChange={(e) => handleChange(e)}/>
        </div>
    )
}

export default ConversationSearch;
