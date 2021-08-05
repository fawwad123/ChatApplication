import axios from 'axios';

const authBaseUrl = "https://localhost:5001/api/Authentication"
const userBaseUrl = "https://localhost:5001/api/User"
const userProfileUrl = authBaseUrl + "/getUserDetails";
const addNewContactUrl = userBaseUrl + "/addNewContact?"
const addNewGroupUrl = userBaseUrl + "/addNewGroup?"
const addContactToGroupUrl = userBaseUrl + "/addUserToGroup?"
const registrationUrl = authBaseUrl + "/registerUser";

export const userLogin =  (formData) => {
    return new Promise(async (resolve, reject)=>{
        try{
            const url = authBaseUrl+"/authenticateUser?email="+formData.values.email+"&password="+formData.values.password;
            const response = await axios.post(url);
            resolve(response);
        }
        catch(error){
            reject(error.response);
        }
    })
};

export const fetchUser =  () => {
    return new Promise(async (resolve, reject)=>{
        try{
            const token = sessionStorage.getItem('token');
            
            if(!token)
                reject("Token not found!");

            const response = await axios.get(userProfileUrl, {
                headers: {
                    Authorization: "Bearer "+token,
                },
            });
            resolve(response);
        }
        catch(error){
            if(error.response && error.response.status === 401)
                sessionStorage.removeItem('token')
            reject(error.response);
        }
    })
};

export const registerUser =  (formData) => {
    return new Promise(async (resolve, reject)=>{
        try{
            const response = await axios.post(registrationUrl, formData.values);
            resolve(response);
        }
        catch(error){
            reject(error.response);
        }
    })
};

export const addNewContact = (formData) => {
    return new Promise(async (resolve, reject)=>{
        try{
            const token = sessionStorage.getItem('token');
            if(!token)
                reject("Token not found!");

            const url = addNewContactUrl+'email='+formData.email+'&userId='+formData.userId;
            const response = await axios.post(url, null,{
                headers: {
                    Authorization: "Bearer "+token,
                },
            });
            resolve(response);
        }
        catch(error){
            reject(error.response);
        }
    })
}

export const addNewGroup = (formData) => {
    return new Promise(async (resolve, reject)=>{
        try{
            const token = sessionStorage.getItem('token');
            if(!token)
                reject("Token not found!");

            const url = addNewGroupUrl+'groupName='+formData.groupName+'&userId='+formData.userId;
            const response = await axios.post(url, null,{
                headers: {
                    Authorization: "Bearer "+token,
                },
            });
            resolve(response);
        }
        catch(error){
            reject(error.response);
        }
    })
}

export const addContactToGroup = (formData) => {
    return new Promise(async (resolve, reject)=>{
        try{
            const token = sessionStorage.getItem('token');
            if(!token)
                reject("Token not found!");

            const url = addContactToGroupUrl+'email='+formData.email+'&userId='+formData.userId+'&groupId='+formData.groupId;
            const response = await axios.post(url, null,{
                headers: {
                    Authorization: "Bearer "+token,
                },
            });
            resolve(response);
        }
        catch(error){
            reject(error.response);
        }
    })
}