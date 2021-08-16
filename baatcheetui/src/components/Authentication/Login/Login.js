import React, { useState, useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux';
import Loader from "react-loader-spinner";
import { loginPending, loginSuccess, loginFailed } from '../../../slice/loginSlice';
import { userLogin } from '../../../api/userApi';
import { getUserProfile } from '../../../actions/userAction';
import { validate } from "./LoginFormValidation";
import loginImage from '../../../images/login.svg';
import { useHistory } from 'react-router-dom';
import './../Authentication.scss'

const Login = (props) => {

    const dispatch = useDispatch();
    const history = useHistory();
    const [values, setValues] = useState({});
    const [errors, setErrors] = useState({});
    const [isSubmit, setIsSubmit] = useState(false);
    const {isLoadingLogin, error} = useSelector((state) => state.login);
    
    useEffect(() => {
        const fetchData = async () =>{
            try{
                if (Object.keys(errors).length === 0 && isSubmit) {
                    dispatch(loginPending());
                    const token = await userLogin({ values });
                    sessionStorage.setItem('token', token.data);
                    dispatch(loginSuccess());
                    dispatch(getUserProfile());
                    history.push('/dashboard');
                }
            }catch(error){
                dispatch(loginFailed(error))
                setIsSubmit(false)
            }
        }
        fetchData();
    }, [errors, isSubmit, dispatch, history, values]);

    const handleSubmit = (e) => {
        if(e)  e.preventDefault();
        setErrors(validate(values))
        setIsSubmit(true);
    };

    const handleChange = (e) => {
        e.persist();
        setValues(values => ({ ...values, [e.target.name]: e.target.value }));
    }

    return (
        <div className="base-container">
            <div className="header">Login</div>
            {error !== '' ? <p className="alert alert-danger">{error.data}</p> : ''}
            <div className="content">
                <div className="image">
                    <img src={loginImage} alt='Logo'/>
                </div>
                <div className="form">
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
                    <div className="footer">
                        {isLoadingLogin ? <Loader type="Puff" color="#00BFFF" height={50}  width={50} /> : ''}
                        <button type="submit" className="btn" onClick={(e) => handleSubmit(e)}>Login</button>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default Login;