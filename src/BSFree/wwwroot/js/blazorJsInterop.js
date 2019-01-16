var appStateRef;

window.initAppStateRef = instance => (appStateRef = instance);

window.onscroll = () => {
  if (window.innerHeight + window.scrollY >= document.body.offsetHeight) {
    appStateRef.invokeMethodAsync("GetNextShoutsPage");
  }
};
