import React, { useState, useEffect, useRef } from 'react'
import './NavIcon.scss'
export const NavIcon = (props) => {
    const [open, setOpen] = useState(false)
    let menuRef = useRef();
    
    useEffect(() => {
        document.addEventListener('mousedown', (event) => {
            if(!menuRef.current.contains(event.target)){
                setOpen(false)
            }
        })
    })
    
    return (
        <div ref={menuRef}>
            <li className='nav-item'>
                <a href='#' className="icon-button" onClick={() =>  setOpen(!open)}>
                    <i className="fa fa-ellipsis-v"  aria-hidden="true"></i>
                </a>
                {open && props.children}
            </li>
        </div>
    )
}
