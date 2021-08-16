import React, { useState } from 'react'
import { CSSTransition } from 'react-transition-group'
import ChangePassword from './ChangePassword'
import { DropdownItem } from './DropdownItem'
import './DropdownMenu.scss'
import Profile from './Profile'

export const DropdownMenu = (props) => {
    const {person, signOut} = props
    const [activeMenu, setActionMenu] = useState('main')
    const [menuHeight, setMenuHeight] = useState(null)

    const calHeight = (el) => {
        const height = el.offsetHeight
        setMenuHeight(height+20)
    }
    return (
        <div className="dropdown" style={{height: menuHeight}}>
            <CSSTransition 
                in={activeMenu === 'main'} 
                unmountOnExit 
                timeout={500}
                classNames='menu-primary'
                onEnter={calHeight}>
                <div className='menu'>
                    <DropdownItem
                        children='My Profile'
                        leftIcon={<i className="fa fa-user-circle"></i>}
                        goToMenu='profile'
                        setActionMenu={setActionMenu}>
                    </DropdownItem>
                    <DropdownItem
                        children='Change Password'
                        leftIcon={<i className="fa fa-lock"></i>}
                        goToMenu='changePassword'
                        setActionMenu={setActionMenu}>
                    </DropdownItem>
                    <DropdownItem
                        children='Logout'
                        leftIcon={<i className="fa fa-sign-out"></i>}
                        clickFunc={() => signOut()}>
                    </DropdownItem>
                </div>
            </CSSTransition>
            <Profile person={person} activeMenu={activeMenu} calHeight={calHeight} setActionMenu={setActionMenu}/>
            <ChangePassword activeMenu={activeMenu} calHeight={calHeight} setActionMenu={setActionMenu}/>
            
        </div>
    )
}
