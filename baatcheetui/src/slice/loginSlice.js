import { createSlice } from '@reduxjs/toolkit';
import logger from 'redux-logger'

const initialState = {
    isLoadingLogin: false,
    isAuth: false,
    error: ''
}

const loginSlice = createSlice({
    name: 'login',
    initialState,
    reducers: {
        loginPending: (state) => {
            state.isLoadingLogin = true;
        },
        loginSuccess: (state) => {
            state.isLoadingLogin = false;
            state.isAuth = true;
            state.error = '';
        },
        loginFailed: (state, action) => {
            state.isLoadingLogin = false;
            state.error = action.payload;
        }
    },
    middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(logger),
});

const {reducer, actions} = loginSlice;
export const {loginPending, loginSuccess, loginFailed } = actions;
export default reducer;