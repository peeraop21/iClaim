<template>
    <div id="app">

        <nav-menu></nav-menu>

        <router-view />


    </div>
</template>

<script>

    import NavMenu from './components/_Layout/NavMenu.vue'

    export default {
        components: {
            NavMenu
        },

        name: 'App',
        data() {
            return {

            }
        },
        methods: {


        },
        mounted() {


        },
        created() {
            if (process.env.NODE_ENV == "production") {
                //this.$store.state.envUrl = "https://ts2digitalclaim.rvp.co.th" // demo
                //this.$store.state.liffId = "1655252355-n08QYdAA" //demo

                //this.$store.state.envUrl = "https://iclaimapi.rvpeservice.com" // UAT
                //this.$store.state.liffId = "1655252355-Jla4mbZZ" //UAT

                this.$store.state.envUrl = "https://iclaimapi.rvpeservice.com" // prod
                this.$store.state.liffId = "1655252355-Jla4mbZZ" //prod
                

            } else if (process.env.NODE_ENV == "development") {
                this.$store.state.envUrl = "http://localhost:50598"
                this.$store.state.liffId = "1655252355-n08QYdAA" //demo

            }
            
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
