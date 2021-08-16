import React from 'react'
import './DropdownItem.scss'
export const DropdownItem = (props) => {
    const {leftIcon, rightIcon, children, goToMenu, setActionMenu, clickFunc} = props
    return (
        <a href="#" className='menu-item' 
            onClick={() => {
                goToMenu ? setActionMenu(goToMenu) : clickFunc()
            }}>
            <span className='icon-item'>{leftIcon}</span>
            <div className='icon-item'>{children}</div>
            <span className='icon-item'>{rightIcon}</span>
        </a>
    )
}
