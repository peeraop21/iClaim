<template>
    <dev class="container" align="center">
        <div class="container">
            <div class="row">
                <div class="col-12">
                    <br><br>
                    <a class="btn-intro" href="default.asp">สิทธิ์คุ้มครองตาม พ.ร.บ.</a>
                </div>
            </div>
            <br>
            <div class="row">
                <div class="col-12">
                    <br><br>
                    <a class="btn-intro" href="default.asp">ขั้นตอนดำเนินการ<br>และเอกสารที่ต้องเตรียม</a>
                </div>
            </div>
            <br>
            <div class="row">
                <div class="col-12">
                    <br><br>
                    <a class="btn-next" href="\Accident" >{{ msg }}</a>

                </div>
            </div>
        </div>
    </dev>
</template>
<script>
    export default {
        name: 'Home',
        data() {
            return {
                msg: "ดำเนินการต่อ"
            }
        }
    }
</script>
<style>
    a[class="btn-intro"]:link, a[class="btn-intro"]:visited {
        background-color: white;
        color: black;
        border: 2px solid #5c2e91;
        padding: 7% 20px;
        width: 70%;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        font-size: 20px;
    }

    a[class="btn-intro"]:hover, a[class="btn-intro"]:active {
        background-color: #5c2e91;
        color: white;
    }

    a[class="btn-next"]:link, a[class="btn-next"]:visited {
        background-color: #5c2e91;
        color: white;
        padding: 5px 50px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 30px;
        font-size: 20px;
    }

    a[class="btn-next"]:hover, a[class="btn-next"]:active {
        background-color: #50287e;
    }
</style>