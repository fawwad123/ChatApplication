import { configureStore } from '@reduxjs/toolkit';
import loginReducer from '../slice/loginSlice';
import userReducer from '../slice/userSlice';
import registerReducer from "../slice/registerSlice";
import changePasswordReducer from '../slice/changePasswordSlice';

export default configureStore({
    reducer: {
        login: loginReducer,
        user: userReducer,
        register: registerReducer,
        changePassword: changePasswordReducer
    },
});