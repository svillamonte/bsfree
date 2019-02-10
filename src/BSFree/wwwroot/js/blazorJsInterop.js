const TOP_MARGIN = 60;

var appStateRef;

window.initAppStateRef = instance => (appStateRef = instance);

window.scrollToShout = shoutId => {
  const attributes = document
    .getElementById("shout-" + shoutId)
    .getBoundingClientRect();

  window.scrollTo(0, attributes.top - TOP_MARGIN);
};

window.onscroll = () => {
  if (window.innerHeight + window.scrollY >= document.body.offsetHeight) {
    appStateRef.invokeMethodAsync("GetNextShoutsPage");
  }
};
