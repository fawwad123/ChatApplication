import React from 'react'
import { CSSTransition } from 'react-transition-group'
import { DropdownItem } from './DropdownItem'
import './DropdownMenu.scss'

const ChangePassword = (props) => {
    const {activeMenu, calHeight, setActionMenu} = props
    return (
        <CSSTransition 
            in={activeMenu === 'changePassword'} 
            unmountOnExit 
            timeout={500}
            classNames='menu-secondary'
            onEnter={calHeight}>
            <div className='menu'>
                <DropdownItem
                    leftIcon={<i className="fa fa-arrow-left" ></i>}
                    goToMenu='main'
                    setActionMenu={setActionMenu}>
                </DropdownItem>
                <div className="form-group">
                    <label htmlFor="password">Old Password</label>
                    <input 
                        name="oldPassword"
                        type="password"
                        placeholder="Old Password"  
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="password">Password</label>
                    <input 
                        name="password"
                        type="password"
                        placeholder="Password"  
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="changePassword">Change Password</label>
                    <input 
                        name="changePassword"
                        type="password"
                        placeholder="Change Password"  
                    />
                </div>
                <button className='edit-button' style={{fontWeight:'bold'}}>Save Changes</button>
            </div>
        </CSSTransition>
    )
}
export default ChangePassword;