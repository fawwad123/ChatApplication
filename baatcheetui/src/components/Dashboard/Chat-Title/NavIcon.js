import React, { useState } from 'react'
import './NavIcon.scss'
export const NavIcon = (props) => {
    const [open, setOpen] = useState(false)
    return (
        <li className='nav-item'>
            <a href='#' className="icon-button" onClick={() =>  setOpen(!open)}>
                <i className="fa fa-ellipsis-v"  aria-hidden="true"></i>
            </a>
            {open && props.children}
        </li>
    )
}
