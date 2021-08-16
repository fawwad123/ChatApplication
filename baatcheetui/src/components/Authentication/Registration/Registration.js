import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import DatePicker from 'react-datepicker';
import Loader from "react-loader-spinner";
import "react-datepicker/dist/react-datepicker.css";
import { validate } from "./RegistrationFormValidation";
import { registrationPending, registrationSuccess, registrationFailed } from '../../../slice/registerSlice';
import registrationImages from '../../../images/registration.svg';
import './../Authentication.scss';
import { registerUser } from '../../../api/userApi';

const Registration = (props) => {
    const dispatch = useDispatch();
    const [values, setValues] = useState({});
    const [errors, setErrors] = useState({});
    const [response, setResponse] = useState("");
    const [image, setImage] = useState("");
    const {isLoadingRegistration, error} = useSelector((state) => state.register);

    const handleFileChange = async (e) => {
        if(e) e.persist();
        const file = e.target.files[0]
        if(file){
            const base64 = await convertBase64(file);
            setImage(base64)
            setValues(values => ({ ...values, [e.target.name]: base64.toString() }));
        }
        else
            setImage('')
    }

    const convertBase64 = (file) => {
        return new Promise((resolve, reject) => {
            const fileReader = new FileReader();
            fileReader.readAsDataURL(file);
            fileReader.onload = () => {
                resolve(fileReader.result);
            }
            fileReader.onerror = (error) => {
                reject(error)
            }
        })
    }

    const handleChange = (e) => {
        if(e) e.persist();
        setValues(values => ({ ...values, [e.target.name]: e.target.value }));
    }

    const dateChange = (date) => {
        setValues(values => ({ ...values, 'dateOfBirth': date }));
        setValues(values => ({ ...values, 'isActive': true }));
        setValues(values => ({ ...values, 'modifiedOn': new Date() }));
        setValues(values => ({ ...values, 'createdOn': new Date() }));
    }

    const handleSubmit = async (e) =>{
        try {
            if(e)  e.preventDefault();
            var errors = validate(values)
            setErrors(errors)
            if(Object.keys(errors).length === 0){
                dispatch(registrationPending())
                const response = await registerUser({values})
                dispatch(registrationSuccess())
                setResponse(response.data)
                setValues({})
            }
        } catch (error) {
            dispatch(registrationFailed(error))
        }
    }

    return (
        <div className="base-container">
            <div className="header">Registration</div>
            {error !== '' ? <p className="alert alert-danger">{error}</p> : ''}
            {response != '' ? <p className="alert alert-success">{response}</p> : ''}
            <div className="content">
                <div className="image">
                    <img src={registrationImages} alt='logo'/>
                </div>
                {image == ''?'':<img src={image} style={{height:150}}/>}
                <div className="form">
                    <div className="form-group">
                        <input 
                            name="imageUrl"
                            type="file" 
                            onChange={(e) => handleFileChange(e)} 
                        />
                        {errors.imageUrl && ( <p className="alert alert-danger"><strong>{errors.imageUrl}</strong></p>)}
                    </div>
                    <div className='row'>
                        <div className="form-group">
                            <label htmlFor="name">Name</label>
                            <input 
                                name="name"
                                type="text" 
                                placeholder="Name" 
                                value={values.name || ''} 
                                onChange={(e) => handleChange(e)} 
                            />
                            {errors.name && ( <p className="alert alert-danger"><strong>{errors.name}</strong></p>)}
                        </div>
                        <div className="form-group">
                            <label htmlFor="firstName">First Name</label>
                            <input 
                                name="firstName"
                                type="text" 
                                placeholder="First Name" 
                                value={values.firstName || ''} 
                                onChange={(e) => handleChange(e)} 
                            />
                            {errors.firstName && ( <p className="alert alert-danger"><strong>{errors.firstName}</strong></p>)}
                        </div>
                    </div>
                    <div className='row'>
                        <div className="form-group">
                            <label htmlFor="middleName">Middle Name</label>
                            <input 
                                name="middleName"
                                type="text" 
                                placeholder="Middle Name" 
                                value={values.middleName || ''} 
                                onChange={(e) => handleChange(e)} 
                            />
                        </div>
                        <div className="form-group">
                            <label htmlFor="LastName">Last Name</label>
                            <input 
                                name="lastName"
                                type="text" 
                                placeholder="Last Name" 
                                value={values.lastName || ''} 
                                onChange={(e) =>handleChange(e)} 
                            />
                            {errors.lastName && ( <p className="alert alert-danger"><strong>{errors.lastName}</strong></p>)}
                        </div>
                    </div>
                    <div className='row'>
                        <div className="form-group">
                            <label htmlFor="dateOfBirth">Date Of Birth</label>
                            <DatePicker 
                                name="dateOfBirth"
                                onChange={(date) => dateChange(date)}
                                selected = {values.dateOfBirth || dateChange(new Date())}
                                format = "yyyy-MM-dd"
                            />
                            {errors.dateOfBirth && ( <p className="alert alert-danger"><strong>{errors.dateOfBirth}</strong></p>)}
                        </div>
                        <div className="form-group">
                            <label htmlFor="email">Email</label>
                            <input 
                                name="email"
                                type="email" 
                                placeholder="Email" 
                                value={values.email || ''} 
                                onChange={(e) => handleChange(e)} 
                            />
                            {errors.email && ( <p className="alert alert-danger"><strong>{errors.email}</strong></p>)}
                        </div>
                    </div>
                    <div className='row'>
                        <div className="form-group">
                            <label htmlFor="password">Password</label>
                            <input 
                                name="password"
                                type="password" 
                                placeholder="password" 
                                value={values.password || ''} 
                                onChange={(e) => handleChange(e)} 
                            />
                            {errors.password && ( <p className="alert alert-danger"><strong>{errors.password}</strong></p>)}
                        </div>
                        <div className="form-group">
                            <label htmlFor="ConfirmPassword">Confirm Password</label>
                            <input 
                                name="confirmPassword"
                                type="password" 
                                placeholder="Confirm Password" 
                                value={values.confirmPassword || ''} 
                                onChange={(e) => handleChange(e)} 
                            />
                            {errors.confirmPassword && ( <p className="alert alert-danger"><strong>{errors.confirmPassword}</strong></p>)}
                        </div>
                    </div>
                    <div className="footer">
                    {isLoadingRegistration ? <Loader type="Puff" color="#00BFFF" height={50}  width={50} /> : ''}
                    <button type="submit" className="btn" onClick={(e) => handleSubmit(e)}>Register</button>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default Registration;