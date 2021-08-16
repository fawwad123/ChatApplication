import React from 'react'
import Moment from 'react-moment';
import { CSSTransition } from 'react-transition-group'
import { DropdownItem } from './DropdownItem'
import './DropdownMenu.scss'

const Profile = (props) => {
    const {person, activeMenu, calHeight, setActionMenu} = props
    return (
        <CSSTransition 
            in={activeMenu === 'profile'} 
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
                <div className="profile">
                    <img src={person.imageUrl} alt='proImg'/>
                    <div className='profile-details'>
                        <div className="form-dm">
                            <div className='row'>
                                <div className="form-group-label">
                                    <label>Name</label>
                                </div>
                                <label>:</label>
                                <div className="form-group-value">
                                    <label>{person.name}</label>
                                </div>
                            </div>
                            <div className='row'>
                                <div className="form-group-label">
                                    <label>Email</label>
                                </div>
                                <label>:</label>
                                <div className="form-group-value">
                                    <label>{person.email}</label>
                                </div>
                            </div>
                            <div className='row'>
                                <div className="form-group-label">
                                    <label >First Name</label>
                                </div>
                                <label>:</label>
                                <div className="form-group-value">
                                    <label>{person.firstName}</label>
                                </div>
                            </div>
                            {person.middleName &&
                                <div className='row'>
                                    <div className="form-group-label">
                                        <label>Middle Name</label>
                                    </div>
                                    <label>:</label>
                                        <div className="form-group-value">
                                            <label>{person.middleName}</label>
                                        </div>
                                </div>
                            }
                            <div className='row'>
                                <div className="form-group-label">
                                    <label >Last Name</label>
                                </div>
                                <label>:</label>
                                <div className="form-group-value">
                                    <label>{person.lastName}</label>
                                </div>
                            </div>
                            <div className='row'>
                                <div className="form-group-label">
                                    <label>Date Of Birth</label>
                                </div>
                                <label>:</label>
                                <div className="form-group-value">
                                    <label><Moment format='DD/MM/YYYY'>{person.dateOfBirth}</Moment></label>
                                </div>
                            </div>
                        </div>
                    </div>
                    {/* <button className='edit-button' style={{fontWeight:'bold'}}>Edit Profile</button> */}
                </div>
            </div>
        </CSSTransition>
    )
}
export default Profile;