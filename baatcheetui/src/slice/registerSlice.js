import { createSlice } from '@reduxjs/toolkit';

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
    }
});

const { reducer, actions } = registerSlice;
export const { registrationPending, registrationSuccess, registrationFailed }  = actions;
export default reducer;