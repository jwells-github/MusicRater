const form = document.getElementById('FormDateValidation')
const birthDay = document.getElementById('BirthDay');
const birthMonth = document.getElementById('BirthMonth');
const birthYear = document.getElementById('BirthYear');
const deathhDay = document.getElementById('DeathDay');
const deathMonth = document.getElementById('DeathMonth');
const deathYear = document.getElementById('DeathYear');
const BirthDateError = document.getElementById("BirthDate-Error");
const DeathDateError = document.getElementById("DeathDate-Error");

form.addEventListener('submit', function (event) {
    if (birthDay.value != 0) {
        if (birthMonth.value == 0 || birthYear.value == 0) {
            event.preventDefault();
            BirthDateError.innerHTML = "You must enter a birth month and a birth year in order to submit a day of the month";
        }
    }
    if (birthMonth.value != 0 && birthYear.value == 0) {
        event.preventDefault();
        BirthDateError.innerHTML = "You must enter a birth year in order to submit a birth month";
    }
    if (deathhDay.value != 0) {
        if (birthMonth.value == 0 || birthYear.value == 0) {
            event.preventDefault();
            DeathDateError.innerHTML = "You must enter a death month and a death year in order to submit a day of the month";
        }
    }
    if (deathMonth.value != 0 && deathYear.value == 0) {
        event.preventDefault();
        DeathDateError.innerHTML = "You must enter a death year in order to submit a death month";
    }
});