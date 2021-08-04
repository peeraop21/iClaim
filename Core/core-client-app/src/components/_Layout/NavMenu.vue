<template>
    <div>
        <b-navbar class="navbar-digital-cliam">
            <b-navbar-nav >
                <b-nav-item class="navbar-digital-cliam-title" >Digital Claim</b-nav-item>

                
            </b-navbar-nav>
        </b-navbar>
    </div>
</template>


<style>
    .navbar-digital-cliam {
        background-color: #5c2e91;
        
    }
    .navbar-digital-cliam-title {
        color: white;
    }
</style>
<script>
    export default {
        name: "NavMenu",
        data() {
            return {
                isExpanded: false
            }
        },
        methods: {
            collapse() {
                this.isExpanded = false;
            },

            toggle() {
                this.isExpanded = !this.isExpanded;
            }
        }
    }
</script>