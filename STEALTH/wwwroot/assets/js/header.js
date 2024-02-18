document.querySelectorAll('header .header-mdl .header-btn').forEach(element => {

    element.addEventListener('click', (e) => {
        document.querySelectorAll('header .header-mdl .active').forEach((el) => {
            el.classList.remove("active");
        });

        element.classList.add("active")
    })
});