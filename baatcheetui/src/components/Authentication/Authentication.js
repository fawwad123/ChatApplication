import React, { useEffect, useState } from 'react';
import Login from './Login/Login';
import Registration from './Registration/Registration';
import { RightSide } from './RightSide';
import './Authentication.scss'
import { useDispatch } from 'react-redux';
import { useHistory } from 'react-router-dom';
import { getUserProfile } from '../../actions/userAction';

export const Authentication = () => {

    const [isLoggedInActive, setIsLoggedInActive] = useState(true);
    const history = useHistory();
    const dispatch = useDispatch();
    var current = isLoggedInActive ? "Register" : "Login";
    var [rightSide] = useState('right');
    
    useEffect(() =>{
        document.body.style.display = 'block'
        if(sessionStorage.getItem('token')){
            dispatch(getUserProfile());
            history.push('/dashboard');
        }
    }, [dispatch, history]);

    const changeState= (isActive) => {
        if(isActive){
            rightSide.classList.remove('right');
            rightSide.classList.add('left');
        }
        else{
            rightSide.classList.remove('left');
            rightSide.classList.add('right');
        }
        
        setIsLoggedInActive(!isActive);
    }

    return (
        <div className="Login">
            <div className="container">
                {isLoggedInActive && <Login containerRef={(ref) => current = ref}/>}
                {!isLoggedInActive && <Registration changeState={changeState} containerRef={(ref) => current = ref}/>}
            </div>
            <RightSide current={current} containerRef={(ref) => rightSide = ref} onClick={() => changeState(isLoggedInActive)}/>
        </div>
    )
} 