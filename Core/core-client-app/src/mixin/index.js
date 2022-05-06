import axios from 'axios'


export default {

    methods: {
        async getJwtToken() {
            if (this.$store.state.hasRegistered) {
                var urlJwt = this.$store.state.envUrl + '/api/jwt'
                await axios.post(urlJwt, {
                    SystemName: process.env.VUE_APP_JWT_SYSTEMNAME,
                    Password: process.env.VUE_APP_JWT_PASSWORD,
                    UserId: this.$store.state.userTokenLine
                }).then((response) => {
                    this.$store.state.jwtToken = response.data
                }).catch(function (error) {
                    alert(error)
                })
            }           
        },
    }
}
