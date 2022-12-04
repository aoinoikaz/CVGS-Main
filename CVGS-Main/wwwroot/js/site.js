// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Register page multi step form

let currentPage = 0;

const nextBtns = document.querySelectorAll('.registerFormNextBtns');
const previousBtns = document.querySelectorAll('.registerFormPreviousBtns');
const progress = document.getElementById('progress');
const formSteps  = document.querySelectorAll('.form-step');
const progressSteps = document.querySelectorAll('.progress-bar-step');
const step1Inputs = document.querySelectorAll('.step1-fields');
const step2Inputs = document.querySelectorAll('.step2-fields');
const step3Inputs = document.querySelectorAll('.step3-fields');


previousBtns.forEach(btn =>
{
    btn.addEventListener('click', () => {
        currentPage--;
        UpdateFormSteps();
        UpdateProgressBar();
    });
});
  
nextBtns.forEach(btn =>
{
    btn.addEventListener('click', () =>
    {
        switch (currentPage)
        {
            case 0:
                MoveForward();
                break;
            case 1:
                MoveForward();
                break;
        }
    });
});

function MoveForward()
{
    currentPage++;
    UpdateFormSteps();
    UpdateProgressBar();
}

function UpdateFormSteps()
{
    formSteps.forEach((formStep) =>
    {
        formStep.classList.contains('form-step-active') && formStep.classList.remove('form-step-active');
    });

formSteps[currentPage].classList.add('form-step-active');
}

function UpdateProgressBar()
{
    progressSteps.forEach((progressStep, i) =>
    {
        if (i < currentPage + 1)
        {
            progressStep.classList.add('progress-bar-step-active');
        }
        else
        {
            progressStep.classList.remove('progress-bar-step-active');
        }
    });

const progressActive = document.querySelectorAll('.progress-bar-step-active');
progress.style.width = ((progressActive.length - 1) / (progressSteps.length - 1)) * 100 + '%';
}

