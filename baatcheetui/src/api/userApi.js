import axios from 'axios';

const authBaseUrl = "https://localhost:5001/api/Authentication"
const userProfileUrl = authBaseUrl + "/getUserDetails";
const registrationUrl = authBaseUrl + "/registerUser";

export const userLogin =  (formData) => {
    return new Promise(async (resolve, reject) => {
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
    return new Promise(async (resolve, reject) => {
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
    return new Promise(async (resolve, reject) => {
        try{
            const response = await axios.post(registrationUrl, formData.values);
            resolve(response);
        }
        catch(error){
            reject(error.response);
        }
    })
};

export const changePassword = (formData) => {
    return new Promise(async (resolve, reject) => {
        try {
            const url = authBaseUrl + '/changePassword?oldPassword='+formData.values.oldPassword+'&newPassword='+formData.values.password;
            const token = sessionStorage.getItem('token');
            
            if(!token)
                reject("Token not found!");
            else{
                const response = await axios.post(url,null, {
                    headers: {
                        Authorization: "Bearer "+token,
                    },
                })
                resolve(response);
            }
        } catch (error) {
            reject(error.response);
        }
    })
}