import React, { useState } from 'react'
import { useDispatch, useSelector } from 'react-redux';
import Modal from 'react-modal';
import Loader from "react-loader-spinner";
import './Modal.scss'
import { addContactToGroup, addNewContact, addNewGroup } from '../../../api/userApi';
import { getUserPending, getUserSuccess, getUserFailed, getUserResponse } from '../../../slice/userSlice';

Modal.setAppElement('#root')
const CustomModal = (props) => {
    const {user, isLoadingUser} = useSelector((state) => state.user)
    const [value, setValue] = useState("");
    const [error, setError] = useState("");
    const [message, setMessage] = useState("");
    const dispatch = useDispatch();
    const {modalIsOpen, setModalIsOpen, isContact, addContact} = props
    const customStyles = {
        content: {
          top: '50%',
          left: '50%',
          right: 'auto',
          bottom: 'auto',
          marginRight: '-50%',
          transform: 'translate(-50%, -50%)',
        },
    };

    const handleSubmit = async (e) => {
        try {
            setError(validate(value))
            if(error === ""){
                dispatch(getUserPending())
                var response
                if(isContact)
                    response = await addNewContact({email: value, userId: user.person.id});
                else
                    if(addContact)
                        response = await addContactToGroup({email: value, userId: user.person.id, groupId: sessionStorage.getItem('groupId')})
                    else
                        response = await addNewGroup({groupName: value, userId: user.person.id});

                if(response.data && response.data.person){
                    isContact ? setMessage("User added successfully") : setMessage("Group created successfully")
                    return dispatch(getUserSuccess(response.data));
                }
                else{
                    setMessage(response.data)
                    return dispatch(getUserResponse())
                }
            }
        } catch (error) {
            console.error(error);
            dispatch(getUserFailed(error))
        }
    };

    const handleChange = (e) =>{
        setValue( e.target.value );
    }

    const validate = (value) => {
        let errors = '';
        if(isContact){
            if (value === "") 
                errors = 'Email address is required';
            else if (!/\S+@\S+\.\S+/.test(value)) 
                errors = 'Email address is invalid';
        }
        else
            if (value === "") 
                errors = 'Please enter group name';

        return errors;
    };

    if(!modalIsOpen && error !== '')
        setError("");
    if(!modalIsOpen && message !== '')
        setMessage("")
    if(!modalIsOpen && value !== '')
        setValue("")

    return (
        <Modal 
            isOpen={modalIsOpen}
            shouldCloseOnOverlayClick={true}
            onRequestClose={()=>setModalIsOpen(false)}
            style={customStyles}>
            <i className="fa fa-times" 
                style={{color:'#3498db',display:'flex', flexDirection:'row', justifyContent:'flex-end'}}
                onClick={() => setModalIsOpen(false)}></i>
            {isContact ? <h2>Add New Contact</h2> : addContact ?  <h2>Add Contact To Group</h2> : <h2>Add New Group</h2>}
            <div className="modal-form">
                <div className="modal-form-group">
                    {isContact 
                    ? <input 
                        className="modal-field"
                        name="email"
                        type="email" 
                        placeholder="Email" 
                        value={value} 
                        onChange={(e) => handleChange(e)} 
                        />
                    : addContact ? 
                    <input 
                        className="modal-field"
                        name="email"
                        type="email" 
                        placeholder="Email" 
                        value={value} 
                        onChange={(e) => handleChange(e)} 
                        />
                    :<input 
                        className="modal-field"
                        name="text"
                        type="text" 
                        placeholder="Group Name" 
                        value={value} 
                        onChange={(e) => handleChange(e)} />
                    }
                    {error && ( <p className="alert alert-danger"><strong>{error}</strong></p>)}
                    {message && ( <p className="alert alert-success"><strong>{message}</strong></p>)}
                </div>
                <div className="footer">
                {isLoadingUser ? <Loader type="Puff" color="#00BFFF" height={50}  width={50} /> : ''}
                    <button type="submit" className="modal-btn" onClick={(e) => handleSubmit(e)}>Add</button>
                </div>
            </div>
        </Modal>
    )
}

export default CustomModal;