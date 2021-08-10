<template>
  <div id="app">
    <nav-menu></nav-menu>
    <router-view/>
  </div>
</template>

<script>
    import NavMenu from './components/_Layout/NavMenu.vue'

export default {
  name: 'App',
  components: {
      NavMenu
  }
}
</script>
<style>
#app {
  font-family: Mitr;
  font-size: 13px;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
    :root {
        --main-color: #5c2e91;
    }


#nav {
  padding: 30px;
}

#nav a {
  font-weight: bold;
  color: #2c3e50;
}

#nav a.router-link-exact-active {
  color: #42b983;
}
</style>
