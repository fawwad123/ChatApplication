import React, { useState } from 'react'
import { useSelector } from 'react-redux';
import Modal from 'react-modal';
import Loader from "react-loader-spinner";
import './Modal.scss'

Modal.setAppElement('#root')
const CustomModal = (props) => {
    const {user, isLoadingUser} = useSelector((state) => state.user)
    const [value, setValue] = useState("");
    const [error, setError] = useState("");
    const {modalIsOpen, setModalIsOpen, isContact, addContact, addContactToGroup, addNewContact, addNewGroup, setModalMessage, modalMessage, selectedId} = props
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

    const handleSubmit = async () => {
        try {
            var err = validate(value)
            setError(validate(value))
            if(err === "" && value !== ""){
                if(isContact){
                    await addNewContact({email: value, userId: user.person.id});
                }
                else{
                    if(addContact)
                        await addContactToGroup({email: value, addedBy: user.person.id, groupId: selectedId})
                    else
                        await addNewGroup({name: value, createdBy: user.person.id});
                }
                    
            }
        } catch (error) {
            console.error(error);
        }
    };

    const handleChange = (e) =>{
        setValue( e.target.value );
    }

    const validate = (value) => {
        let errors = '';
        if(isContact || addContact){
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
    if(!modalIsOpen && modalMessage !== '')
        setModalMessage("")
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
                    {error && ( <p className="alert alert-danger" style={{alignSelf:'center'}}><strong>{error}</strong></p>)}
                    {modalMessage && ( <p className="alert alert-success" style={{alignSelf:'center'}}><strong>{modalMessage}</strong></p>)}
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