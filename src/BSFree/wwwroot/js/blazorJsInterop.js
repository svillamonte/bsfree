var appStateRef;

window.initAppStateRef = instance => (appStateRef = instance);

window.scrollToTop = () => window.scrollTo(0, 0);

window.onscroll = () => {
  if (window.innerHeight + window.scrollY >= document.body.offsetHeight) {
    appStateRef.invokeMethodAsync("GetNextShoutsPage");
  }
};
