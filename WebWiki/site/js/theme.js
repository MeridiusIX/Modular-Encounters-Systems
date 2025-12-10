function docReady(fn) {
  // see if DOM is already available
  if (
    document.readyState === "complete" ||
    document.readyState === "interactive"
  ) {
    // call on next available tick
    setTimeout(fn, 1);
  } else {
    document.addEventListener("DOMContentLoaded", fn);
  }
}

/**
 * Behavior for side bar navigation links when clicked
 */
const sideBarCollapsedNavigationLinks = () => {
  const elementsCollapsed = document.getElementsByClassName(
    "side-bar__link--collapsed"
  );
  const elementsExpanded = document.getElementsByClassName(
    "side-bar__link--expanded"
  );

  const onElementClick = (element) => {
    element.onclick = () => {
      if (element.classList.contains("side-bar__link--collapsed")) {
        element.classList.remove("side-bar__link--collapsed");
        element.classList.add("side-bar__link--expanded");
        const submenu =
          element.parentElement.getElementsByClassName("side-bar__sub-menu");
        submenu[0].classList.add("side-bar__sub-menu--visible");
      } else {
        element.classList.remove("side-bar__link--expanded");
        element.classList.add("side-bar__link--collapsed");
        const submenu =
          element.parentElement.getElementsByClassName("side-bar__sub-menu");
        submenu[0].classList.remove("side-bar__sub-menu--visible");
      }
    };
  };
  Array.prototype.map.call(elementsCollapsed, onElementClick);
  Array.prototype.map.call(elementsExpanded, onElementClick);
};

const openSideBarMenu = () => {
  const menu = document.getElementsByClassName("top-bar__item--trigger")[0];
  menu.onclick = () => {
    const appLayout = document.getElementsByClassName("app-layout")[0];
    appLayout.classList.add("app-layout--active-side-bar");
  };
};

const closeSideBarMenu = () => {
  const menu = document.getElementsByClassName("side-bar__close")[0];
  menu.onclick = () => {
    const appLayout = document.getElementsByClassName("app-layout")[0];
    appLayout.classList.remove("app-layout--active-side-bar");
  };
};

const updateSecondarySideBarHeight = () => {
  // Get width and height of the window excluding scrollbars
  // const w = document.documentElement.clientWidth;
  const h = document.documentElement.clientHeight;

  const secondarySideBar =
    document.getElementsByClassName("sidebar--secondary")[0];
  const secondarySideBarExists = !!secondarySideBar;
  if (secondarySideBarExists) {
    secondarySideBar.style["height"] = `${h - 160}px`;
  }
};

const addNavigationControl = (navigationClass, keyCode) => {
  document.addEventListener("keyup", (event) => {
    if (event.isComposing || event.keyCode !== keyCode) {
      return;
    }
    const elements = document.getElementsByClassName(navigationClass);
    Array.prototype.map.call(elements, (element) => {
      element.click();
    });
  });
};

const KEY_LEFT = 37;
const KEY_RIGHT = 39;

function loadColorScheme() {
  const isSystemDark =
    window.matchMedia("(prefers-color-scheme: dark)").media !== "not all";
  const loadSettings = localStorage.getItem("dark-mode");
  const checked =
    loadSettings !== null ? JSON.parse(loadSettings) : isSystemDark;
  document.getElementById("dark-mode").checked = checked;
}

function saveColorScheme(value) {
  const checkbox = document.getElementById("dark-mode");
  localStorage.setItem("dark-mode", value);
}

function startColorScheme() {
  const checkbox = document.getElementById("dark-mode");
  checkbox.onclick = () => {
    saveColorScheme(checkbox.checked);
  };
}
// load prefered mode
loadColorScheme();

docReady(() => {
  startColorScheme();

  // Attaching the event listener function to window's resize event
  window.addEventListener("resize", updateSecondarySideBarHeight);
  // Initializing functions
  updateSecondarySideBarHeight();
  sideBarCollapsedNavigationLinks();
  openSideBarMenu();
  closeSideBarMenu();
  addNavigationControl("navigation-prev", KEY_LEFT);
  addNavigationControl("navigation-next", KEY_RIGHT);
});
