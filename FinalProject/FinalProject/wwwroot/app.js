const searchContainer = document.querySelector(".search-container");
const microphone = document.querySelector(".mic");
const microphoneMsg = document.querySelector(".voice");

microphone.addEventListener("mouseover", () => {
    gsap.to(microphoneMsg, {
        opacity: 1,
        duration: 1,
        ease: "back"
    });

});
microphone.addEventListener("mouseout", () => {
    gsap.to(microphoneMsg, {
        opacity: 0,
        duration: 1,
        ease: "power2"
    });

});