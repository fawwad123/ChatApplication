import React, { useState, useEffect, useRef } from 'react'
import './NavIcon.scss'
export const NavIcon = (props) => {
    const [open, setOpen] = useState(false)
    let menuRef = useRef();
    
    useEffect(() => {
        document.addEventListener('mousedown', (event) => {
            if(menuRef.current && !menuRef.current.contains(event.target)){
                setOpen(false)
            }
        })
    })
    
    return (
            <li ref={menuRef} className='nav-item'>
                <a href='#' className="icon-button" onClick={() =>  setOpen(!open)}>
                    <i className="fa fa-ellipsis-v"  aria-hidden="true"></i>
                </a>
                {open && props.children}
            </li>
    )
}
