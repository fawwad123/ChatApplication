export function validate(values){
    let errors = {};

    if(!values.oldPassword)
        errors.oldPassword = 'Old password is required';
    else if(values.oldPassword === values.password)
        errors.oldPassword = 'Old password and new password cannot be the same'
    if (!values.password) 
        errors.password = 'Password is required';
    else if (values.password.length < 8)
        errors.password = 'Password must be 8 or more characters';
    if(!values.confirmPassword)
        errors.confirmPassword = 'Change password is required';
    else if(values.password !== values.confirmPassword)
        errors.confirmPassword = 'Password or change password doesn\'t match';
    
    return errors;
}