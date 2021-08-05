import { createSlice } from '@reduxjs/toolkit';

const initialState = {
    user: {},
    isLoadingUser: false,
    error: ''
};

const userSlice = createSlice({
    name:'user',
    initialState,
    reducers: {
        getUserPending: (state) => {
            state.isLoadingUser = true;
        },
        getUserSuccess: (state, action) => {
            state.isLoadingUser = false;
            state.user = action.payload;
            state.error = ''
        },
        getUserResponse: (state) => {
            state.isLoadingUser = false;
        },
        getUserFailed: (state, action) => {
            state.isLoadingUser = false;
            state.error = action.payload;
        },
        clearUser: (state) => {
            state.user = {};
            state.isLoadingUser = false;
            state.error = '';
        }
    },
});

const {reducer, actions} = userSlice;
export const {getUserPending, getUserSuccess, getUserResponse, getUserFailed, clearUser} = actions;
export default reducer;