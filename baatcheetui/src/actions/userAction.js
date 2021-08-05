import { fetchUser } from '../api/userApi';
import { getUserPending, getUserSuccess, getUserFailed } from '../slice/userSlice';

export const getUserProfile = () => async (dispatch) => {

    try {
        dispatch(getUserPending())
        const response = await fetchUser();

        if(response.data && response.data.person.id)
            return dispatch(getUserSuccess(response.data));
        dispatch(getUserFailed("No user Found"));
        
    } catch (error) {
        dispatch(getUserFailed(error))
    }
};