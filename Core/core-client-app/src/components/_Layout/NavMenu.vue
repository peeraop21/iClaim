<template>
    <div>
        <b-navbar class="navbar-digital-cliam">
            <b-navbar-brand style="color:white;padding-left:10px;">Digital Claim</b-navbar-brand>
        </b-navbar>
    </div>
</template>

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

<style>
    .navbar-digital-cliam {
        background-color: #5c2e91;
    }
</style>