const form = document.getElementById('FormDateValidation')
const releaseDay = document.getElementById('ReleaseDay');
const releaseMonth = document.getElementById('ReleaseMonth');
const releaseYear = document.getElementById('ReleaseYear');
const ReleaseDateError = document.getElementById("ReleaseDate-Error");


form.addEventListener('submit', function (event) {
    if (releaseDay.value != 0) {
        if (releaseMonth.value == 0 || releaseYear.value == 0) {
            event.preventDefault();
            ReleaseDateError.innerHTML = "You must enter a release month and a release year in order to submit a day of the month";
        }
    }
    if (releaseMonth.value != 0 && releaseYear.value == 0) {
        event.preventDefault();
        ReleaseDateError.innerHTML = "You must enter a release year in order to submit a release month";
    }
});