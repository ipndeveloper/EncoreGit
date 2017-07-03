function CheckValidDate(dayfield, monthfield, yearfield) 
{
    var dayobj = new Date(yearfield, monthfield - 1, dayfield)
    if ((dayobj.getMonth() + 1 != monthfield) || (dayobj.getDate() != dayfield) || (dayobj.getFullYear() != yearfield))
        return false;
    else
        return true;
}

function CheckValidAge(dayfield, monthfield, yearfield) {
    var age = 18;
    var isAgeValid = false;
    var mydate = new Date();
    var passedDate = new Date(yearfield, monthfield - 1, dayfield)
    var minRequiredCurrdate = new Date();
    minRequiredCurrdate.setFullYear(minRequiredCurrdate.getFullYear() - age);

    isAgeValid = passedDate <= minRequiredCurrdate;
    return isAgeValid;
}
    

