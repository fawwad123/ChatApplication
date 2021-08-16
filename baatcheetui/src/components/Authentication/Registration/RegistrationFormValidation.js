export function validate(values){
    let errors = {};

    if(!values.imageUrl)
        errors.imageUrl = "Please upload image"
    if(!values.name)
        errors.name = 'Name is required';
    if(!values.firstName)
        errors.firstName = 'First name is required';
    if(!values.lastName)
        errors.lastName = 'Last name is required';
    if(values.dateOfBirth.setHours(0,0,0,0) > new Date().setHours(0,0,0,0))
        errors.dateOfBirth = 'Date cannot be greater than today'
    if (!values.email) 
        errors.email = 'Email address is required';
    else if (!/\S+@\S+\.\S+/.test(values.email)) 
        errors.email = 'Email address is invalid';
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