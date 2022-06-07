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
                
                this.$store.state.envUrl = process.env.VUE_APP_API_ICLAIM_URL// demo
                this.$store.state.liffId = process.env.VUE_APP_LIFF_ID //demo

                //this.$store.state.envUrl = "https://iclaimapi.rvpeservice.com" // UAT
                //this.$store.state.liffId = "1655252355-Jla4mbZZ" //UAT

                //this.$store.state.envUrl = "https://iclaimapi.rvpeservice.com" // prod
                //this.$store.state.liffId = "1655252355-Jla4mbZZ" //prod
                

            } else if (process.env.NODE_ENV == "development") {
                console.log(process.env.VUE_APP_LIFF_ID)
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
    .btn-next-submit {
        box-shadow: 3px 3px 5px -4px #888888;
        background-color: var(--main-color);
        color: white;
        padding: 5px 50px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 20px;
        font-size: 15px;
        border: none;
    }
    .shadow-vs .vs-card{
        box-shadow: 3px 3px 5px -4px #888888;
    }
    .shadow-vs-button {
        box-shadow: 3px 3px 5px -4px #888888;
    }
    .wizard-icon-container {
        box-shadow: 3px 3px 5px -4px #888888;
    }
    .white-vs-radio .vs-radio {
        background-color: white !important;
    }
    .box-container {
        box-shadow: 3px 3px 5px -4px #888888;
        background-color: white;
        border: none;
        border-radius: 10px;
        padding: 20px 20px 15px 20px;
        text-align: left;
    }
    .shadow-box {
        box-shadow: 3px 3px 5px -4px #888888;
    }
    .filepond--drop-label {
        box-shadow: 3px 3px 5px -4px #888888;
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
