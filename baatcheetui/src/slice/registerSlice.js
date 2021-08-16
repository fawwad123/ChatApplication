import { createSlice } from '@reduxjs/toolkit';
import logger from 'redux-logger'

const initialState = {
    isLoadingRegistration: false,
    isRegistered: false,
    error: ''
}

const registerSlice = createSlice({
    name: 'registration',
    initialState,
    reducers: {
        registrationPending: (state) => {
            state.isLoadingRegistration = true;
        },
        registrationSuccess: (state) => {
            state.isLoadingRegistration = false;
            state.isRegistered = true;
            state.error = ''
        },
        registrationFailed: (state, action) => {
            state.isLoadingRegistration = false;
            state.isRegistered = false;
            state.error = action.payload;
        }
    },
    middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(logger),
});

const { reducer, actions } = registerSlice;
export const { registrationPending, registrationSuccess, registrationFailed }  = actions;
export default reducer;