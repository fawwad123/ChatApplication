import { createSlice } from '@reduxjs/toolkit';
import logger from 'redux-logger'

const initialState = {
    isLoadingChangePassword: false,
    isPasswordChanged: false,
    error: ''
}

const changePasswordSlice = createSlice({
    name: 'changePassword',
    initialState,
    reducers: {
        changePasswordPending: (state) => {
            state.isLoadingChangePassword = true;
        },
        changePasswordSuccess: (state) => {
            state.isLoadingChangePassword = false;
            state.isPasswordChanged = true;
            state.error =  '';
        },
        changePasswordFailed: (state, action) => {
            state.isLoadingChangePassword = false;
            state.error = action.payload;
        }
    },
    middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(logger),
});

const {reducer, actions} = changePasswordSlice;
export const {changePasswordPending, changePasswordSuccess, changePasswordFailed} = actions;
export default reducer;