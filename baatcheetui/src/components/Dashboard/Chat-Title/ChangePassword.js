import React, { useState } from 'react'
import { CSSTransition } from 'react-transition-group'
import { useDispatch, useSelector } from 'react-redux';
import Loader from "react-loader-spinner";
import { validate } from './ChangePasswordValidation'
import { DropdownItem } from './DropdownItem'
import './DropdownMenu.scss'
import { changePasswordFailed, changePasswordPending, changePasswordSuccess } from '../../../slice/changePasswordSlice';
import { changePassword } from '../../../api/userApi';

const ChangePassword = (props) => {
    const dispatch = useDispatch();
    const {activeMenu, calHeight, setActionMenu} = props
    const [values, setValues] = useState({});
    const [errors, setErrors] = useState({});
    const [response, setResponse] = useState("");
    const { isLoadingChangePassword, error} = useSelector((state) => state.changePassword)

    const handleChange = (e) => {
        if(e) e.persist();
        setValues(values => ({ ...values, [e.target.name]: e.target.value }));
    }

    const handleSubmit = async (e) =>{
        try {
            if(e)  e.preventDefault();
            var errors = validate(values)
            setErrors(errors)
            if(Object.keys(errors).length === 0){
                dispatch(changePasswordPending())
                const response = await changePassword({values})
                dispatch(changePasswordSuccess())
                setResponse(response.data)
                setValues({})
            }
        } catch (error) {
           dispatch(changePasswordFailed(error))
        }
    }
    return (
        <CSSTransition 
            in={activeMenu === 'changePassword'} 
            unmountOnExit 
            timeout={500}
            classNames='menu-secondary'
            onEnter={calHeight}>
            <div className='menu'>
                {error !== '' ? <p className="alert alert-danger">{error}</p> : ''}
                {response != '' ? <p className="alert alert-success">{response}</p> : ''}
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
                        value={values.oldPassword || ''} 
                        onChange={(e) => handleChange(e)}
                    />
                    {errors.password && ( <p className="alert alert-danger"><strong>{errors.oldPassword}</strong></p>)}
                </div>
                <div className="form-group">
                    <label htmlFor="password">Password</label>
                    <input 
                        name="password"
                        type="password"
                        placeholder="Password"  
                        value={values.password || ''} 
                        onChange={(e) => handleChange(e)}
                    />
                    {errors.password && ( <p className="alert alert-danger"><strong>{errors.password}</strong></p>)}
                </div>
                <div className="form-group">
                    <label htmlFor="changePassword">Change Password</label>
                    <input 
                        name="confirmPassword"
                        type="password"
                        placeholder="Change Password" 
                        value={values.confirmPassword || ''} 
                        onChange={(e) => handleChange(e)} 
                    />
                    {errors.confirmPassword && ( <p className="alert alert-danger"><strong>{errors.confirmPassword}</strong></p>)}
                </div>
                {isLoadingChangePassword ? <Loader type="Puff" color="#00BFFF" height={50}  width={50} /> : ''}
                <button className='edit-button' onClick={(e) => handleSubmit(e)} style={{fontWeight:'bold'}}>Save Changes</button>
            </div>
        </CSSTransition>
    )
}
export default ChangePassword;